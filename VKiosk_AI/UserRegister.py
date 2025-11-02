import cv2
import cv2
import time
import mediapipe as mp
from HandGesture import is_HandsUp
from HandTracking import hand_inside_box
from SocketSender import send_gesture

# Mediapipe hand detection 객체
hands_detector = mp.solutions.hands.Hands(static_image_mode=False, max_num_hands=1, min_detection_confidence=0.7)

# 손 든 시간 기록용
hand_up_start = {}
hand_duration_threshold = 3.0  # 3초 이상일 때 등록


def person_in_zone(x1, y1, x2, y2, zone):
    zx1, zy1, zx2, zy2 = zone
    cx, cy = (x1 + x2) // 2, (y1 + y2) // 2
    return zx1 <= cx <= zx2 and zy1 <= cy <= zy2

def user_registration(person_boxes, zone, track_ids, frame):
    rgb = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
    results = hands_detector.process(rgb)

    # 손을 본 사람 track_id 집합
    seen_tids = set()

    if results.multi_hand_landmarks and results.multi_handedness:
        for i, hand_landmarks in enumerate(results.multi_hand_landmarks):
            handedness = results.multi_handedness[i].classification[0].label
            if handedness != "Right":
                continue

            criteria_idx = hand_landmarks.landmark[0]
            h, w, _ = frame.shape
            hand_x, hand_y = int(criteria_idx.x * w), int(criteria_idx.y * h)

            for j, (x1, y1, x2, y2) in enumerate(person_boxes):
                if not person_in_zone(x1, y1, x2, y2, zone):
                    continue

                if not hand_inside_box(hand_x, hand_y, (x1, y1, x2, y2)):
                    continue

                tid = track_ids[j]
                seen_tids.add(tid)  # 이 사람의 손은 관측됨

                if is_HandsUp(hand_landmarks.landmark):
                    if tid not in hand_up_start:
                        hand_up_start[tid] = time.time()
                    else:
                        elapsed = time.time() - hand_up_start[tid]
                        if elapsed >= hand_duration_threshold:
                            print(f"[✔] User {tid} registered after {elapsed:.2f} sec with hand raised.")
                            send_gesture("start")
                            return tid
                else:
                    # 손 내림 → 초기화
                    hand_up_start.pop(tid, None)

    # 이전 프레임에서 등록 시도했지만 이번 프레임에서 손이 안 보이면 초기화
    inactive_tids = set(hand_up_start.keys()) - seen_tids
    for tid in inactive_tids:
        hand_up_start.pop(tid, None)

    return None


def user_unregisteration(current_id, track_ids): 
    if current_id not in track_ids:
        print("User missing")
        send_gesture("disconnect")
        return None
    
    if cv2.waitKey(1) & 0xFF == ord('r'):
        print("User registration force initialized")
        send_gesture("disconnect")
        return None
    
    return current_id
    
def draw_zone(frame, zone):
    x1, y1, x2, y2 = zone
    cv2.rectangle(frame, (x1, y1), (x2, y2), (255, 255, 0), 2)
    cv2.putText(frame, "Step Here", (x1 + 10, y1 - 10),
                cv2.FONT_HERSHEY_SIMPLEX, 0.7, (255, 255, 0), 2)
