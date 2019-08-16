//
//  CoreMLHelper.swift
//  Unity-iPhone
//
//  Created by Yasuo Hasegawa on 2019/08/15.
//  Copyright © 2019 Yasuo Hasegawa. All rights reserved.
//

import UIKit
import ARKit
import Vision
import Foundation

public class CoreMLHelper : NSObject {
    
    private let currentMLModel = Inceptionv3().model
    private let serialQueue = DispatchQueue(label: "com.unityvisionmlsample.dispatchqueueml")
    private var visionRequests = [VNRequest]()
    private var timer = Timer()
    
    // Classification results
    private var identifierString = ""
    private var confidence: VNConfidence = 0.0
    
    var session:ARSession?
    
    @objc public init(arsession: ARSession) {
        self.session = arsession
    }

    @objc public func setUp() {
        setupCoreML()
        
        timer = Timer.scheduledTimer(timeInterval: 0.1, target: self, selector: #selector(self.loopCoreMLUpdate), userInfo: nil, repeats: true)
    }
    
    private func setupCoreML() {
        guard let selectedModel = try? VNCoreMLModel(for: currentMLModel) else {
            fatalError("Could not load model.")
        }
        
        let classificationRequest = VNCoreMLRequest(model: selectedModel,
                                                    completionHandler: classificationCompleteHandler)
        
        // Crop from centre of images and scale to appropriate size.
        classificationRequest.imageCropAndScaleOption = VNImageCropAndScaleOption.centerCrop
        
        // Use CPU for Vision processing to ensure that there are adequate GPU resources for rendering.
        classificationRequest.usesCPUOnly = true
        visionRequests = [classificationRequest]
    }
    
    private func updateCoreML() {
        let pixbuff : CVPixelBuffer? = (session?.currentFrame?.capturedImage)
        if pixbuff == nil { return }
        
        let deviceOrientation = UIDevice.current.orientation.getImagePropertyOrientation()
        let imageRequestHandler = VNImageRequestHandler(cvPixelBuffer: pixbuff!, orientation: deviceOrientation,options: [:])
        do {
            try imageRequestHandler.perform(self.visionRequests)
        } catch {
            print(error)
        }
    }
    
    @objc private func loopCoreMLUpdate() {
        serialQueue.async {
            self.updateCoreML()
        }
    }
    
    private func classificationCompleteHandler(request: VNRequest, error: Error?) {
        if error != nil {
            print("Error: " + (error?.localizedDescription)!)
            return
        }
        guard let observations = request.results else {
            return
        }
        
        let classifications = observations as! [VNClassificationObservation]
        
        // Show a label for the highest-confidence result (but only above a minimum confidence threshold).
        if let bestResult = classifications.first(where: { result in result.confidence > 0.5 }),
            let label = bestResult.identifier.split(separator: ",").first {
            identifierString = String(label)
            confidence = bestResult.confidence
        } else {
            identifierString = ""
            confidence = 0
        }
        
        DispatchQueue.main.async {
            //print("identifierString: \(self.identifierString) - confidence: \(String(describing: self.confidence))")
            UnitySendMessage("Scripts", "ClassificationResult", self.identifierString);
        }
        
    }
}
