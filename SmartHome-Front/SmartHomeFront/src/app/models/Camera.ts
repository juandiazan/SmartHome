export interface Camera {
    cameraName: string;
    cameraModel: string;
    description: string;
    photos: string[];
    deviceType: string;
    canBeUsedIndoors: boolean;
    canBeUsedOutdoors: boolean;
    hasMovementDetectionSupport: boolean;
    hasPersonDetectionSupport: boolean;
}