from ultralytics import YOLO
from deep_sort_realtime.deepsort_tracker import DeepSort
import cv2
import torch

# Check if GPU is available
USE_CUDA = torch.cuda.is_available()

yolo_model = YOLO("yolov8n.pt")

if USE_CUDA:
    yolo_model.to("cuda")
    print("YOLO model is running on GPU.")
else:
    print("Running YOLO on CPU because I don't have a GPU. It can be slow.")

tracker = DeepSort(max_age=15) # DeepSort tracker

conf_criteria = 0.5
min_weight = 60
min_height = 100


def detect_persons(frame):
    # Detect person using YOLO per frame
    results = yolo_model(frame, verbose=False)[0]
    boxes = []

    # Check bound boxes of people
    for r in results.boxes:
        if yolo_model.names[int(r.cls[0])] == "person":
            boxes.append(list(map(int, r.xyxy[0])))
    return boxes

def draw_person_boxes(frame, boxes, registered_id, track_ids):
    for i, (x1, y1, x2, y2) in enumerate(boxes):
        tid = track_ids[i]
        color = (0, 255, 0) if tid == registered_id else (0, 0, 255)
        label = f"User {i}" if tid == registered_id else f"Person {i}"
        cv2.rectangle(frame, (x1, y1), (x2, y2), color, 2)
        cv2.putText(frame, label, (x1, y1 - 10),
                    cv2.FONT_HERSHEY_SIMPLEX, 0.8, color, 2)

def track_persons(frame):
    # Detect person using YOLO per frame
    results = yolo_model(frame, verbose=False)[0]
    detections = []

    # Check all detected objects
    for result in results.boxes:
        
        # Check Person
        if yolo_model.names[int(result.cls[0])] == "person":
            x1, y1, x2, y2 = map(int, result.xyxy[0])
            w, h = x2 - x1, y2 - y1
            conf = float(result.conf[0])    # Confidence Score

            # Ignore Condition
            if conf < conf_criteria:
                continue
            if w < min_weight or h < min_height:
                continue

            detections.append(([x1, y1, w, h], conf, 'person')) # For DeepSort
            
    tracks = tracker.update_tracks(detections, frame = frame) # Tracking

    person_boxes = []
    track_ids = []

    for track in tracks:    
        if not track.is_confirmed():
            continue
        x1, y1, x2, y2 = map(int, track.to_ltrb())
        person_boxes.append([x1, y1, x2, y2])
        track_ids.append(int(track.track_id))

    return person_boxes, track_ids