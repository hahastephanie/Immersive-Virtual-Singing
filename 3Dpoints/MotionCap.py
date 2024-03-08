import cv2
from cvzone.PoseModule import PoseDetector

cap = cv2.VideoCapture('dance.mov')

detector = PoseDetector()
posList = []

while True:
    success, img = cap.read()
    img = detector.findPose(img)
    lmList, bboxInfo = detector.findPosition(img)

    if bboxInfo:
        if bboxInfo["bbox"][2] > 0 and bboxInfo["bbox"][3] > 0:
            cv2.circle(img, bboxInfo["center"], 5, (255, 0, 0), cv2.FILLED)
            boxW=bboxInfo["bbox"][2]
            boxH = bboxInfo["bbox"][3]
            # print(bboxInfo["bbox"])

            if(boxW<120):
                boxW=120
            if (boxH < 410):
                boxH = 410

            lmString = ''
            for lm in lmList:
                lmx = (lm[1] - bboxInfo["center"][0]) / boxW
                lmy = (img.shape[0] - lm[2] - bboxInfo["center"][1]) / boxH
                lmString += f'{lmx},{lmy},{lm[3]},'

            # print(lmString)
            posList.append(lmString)

    # print(len(posList))

    cv2.imshow("Image", img)
    if cv2.waitKey(1) & 0xFF == ord('s'):
        print('y')
        with open("MotionFile.txt", 'w') as f:
            f.writelines(["%s\n" % item for item in posList])
