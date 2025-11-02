TAP_THRESHOLD = 30
SWIPE_THRESHOLD = 100

def is_grab(landmarks):
    tips = [8, 12, 16, 20]
    return all(landmarks[tip].y > landmarks[tip - 2].y for tip in tips)

def is_swip(pre_x, cur_x, pre_y, cur_y, landmarks):
    tips = [8, 12, 16, 20]
    folded_finger = sum(landmarks[tip].y > landmarks[tip - 2].y for tip in tips)
    
    if folded_finger == 0:
        dx = cur_x - pre_x
        dy = cur_y - pre_y
        if abs(dx) > SWIPE_THRESHOLD:
            if dx > 0:
                return "right"
            elif dx < 0:
                return "left"
            
        if abs(dy) > SWIPE_THRESHOLD:
            if dy > 0:
                return "down"
            elif dy < 0:
                return "up"
            
def is_index(landmarks):
    return landmarks[8].y < landmarks[6].y and all(landmarks[tip].y > landmarks[tip - 2].y for tip in [12, 16, 20])

def is_HandsUp(landmarks):
    tips = [8, 12, 16, 20]
    return all(landmarks[tip].y < landmarks[tip-2].y for tip in tips)