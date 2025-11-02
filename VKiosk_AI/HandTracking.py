import cv2
import pyautogui
import mediapipe as mp
from HandGesture import is_grab, is_swip, is_index
from SocketSender import send_gesture

# Mediapipe Hand / Mediapipe Draw
mp_hands = mp.solutions.hands
hands = mp_hands.Hands(static_image_mode=False, max_num_hands=1, min_detection_confidence=0.7)
mp_draw = mp.solutions.drawing_utils

# Screen : Weight / Height
screen_w, screen_h = pyautogui.size()

# Previous Position
prev_positions = {}

# State
prev_state = None

# Judge hand inside box
def hand_inside_box(x, y, box):
    x1, y1, x2, y2 = box
    return x1 <= x <= x2 and y1 <= y <= y2

def detect_hands(frame, person_boxes, registered_id, track_ids):
    global prev_state
    cur_state = None

    rgb = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
    results = hands.process(rgb)
    
    if results.multi_hand_landmarks and results.multi_handedness:
        for i, hand_landmarks in enumerate(results.multi_hand_landmarks):

            handedness = results.multi_handedness[i].classification[0].label

            if handedness != "Right":
                continue  # Ignore left hand

            criteria_idx = hand_landmarks.landmark[0]                           # Get wrist
            h, w, _ = frame.shape                                               # Get frame shape
            hand_x, hand_y = int(criteria_idx.x * w), int(criteria_idx.y * h)   # Coordinate transformation

            matched = False                                   

            for j, box in enumerate(person_boxes):              # Check all person box and link hand
                if hand_inside_box(hand_x, hand_y, box):        
                    if track_ids[j] == registered_id:
                        matched = True
                        break
            
            # Check registered person
            if matched:
                #mp_draw.draw_landmarks(frame, hand_landmarks, mp_hands.HAND_CONNECTIONS)
                key = f"hand_{registered_id}"

                if key in prev_positions:
                    prev_x, prev_y = prev_positions[key]

                    if is_grab(hand_landmarks.landmark):
                        if prev_state == "index":
                            pyautogui.click()
                            cur_state = "click"
                        else:
                            cur_state = "grab"
                        if cur_state != prev_state:
                            send_gesture(cur_state)
                            print(cur_state)
                        
                    elif is_swip(prev_x, hand_x, prev_y, hand_y, hand_landmarks.landmark) == "right":
                        cur_state = "right_swipe"
                        if prev_state != "left_swipe" and prev_state != "up_swipe" and prev_state != "down_swipe" and cur_state != prev_state:
                            send_gesture(cur_state)
                            print(cur_state)

                    elif is_swip(prev_x, hand_x, prev_y, hand_y, hand_landmarks.landmark) == "left":
                        cur_state = "left_swipe"
                        if prev_state != "right_swipe" and prev_state != "up_swipe" and prev_state != "down_swipe" and cur_state != prev_state:
                            send_gesture(cur_state)
                            print(cur_state)
                        
                    elif is_swip(prev_x, hand_x, prev_y, hand_y, hand_landmarks.landmark) == "up":
                        cur_state = "up_swipe"
                        if prev_state != "left_swipe" and prev_state != "right_swipe" and prev_state != "down_swipe" and cur_state != prev_state:
                            send_gesture(cur_state)
                            print(cur_state)
                        
                    elif is_swip(prev_x, hand_x, prev_y, hand_y, hand_landmarks.landmark) == "down":
                        cur_state = "down_swipe"
                        if prev_state != "right_swipe" and prev_state != "up_swipe" and prev_state != "left_swipe" and cur_state != prev_state:
                            send_gesture(cur_state)
                            print(cur_state)  

                    elif is_index(hand_landmarks.landmark):
                        cur_state = "index"
                        if cur_state != prev_state:
                            if prev_state == "grab":
                                send_gesture("drop")
                            print(cur_state)
                        
                    else:
                        # Drop
                        if prev_state == "grab":
                            cur_state = "drop"
                            if cur_state != prev_state:
                                send_gesture(cur_state)
                                print("drop")  

                        # Default
                        else:
                            cur_state = "default"
                            if cur_state != prev_state:
                                print("default") 

                    prev_state = cur_state  # Update State

                    screen_x = int(criteria_idx.x * screen_w)   
                    screen_y = int(criteria_idx.y * screen_h)
                    
                    pyautogui.moveTo(screen_x, screen_y, duration=0)

                prev_positions[key] = (hand_x, hand_y)
    