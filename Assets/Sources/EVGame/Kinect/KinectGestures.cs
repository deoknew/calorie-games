using UnityEngine;
using System.Collections;

public class KinectGestures
{
	public interface GestureListenerInterface
	{
		// Invoked when a new user is detected and tracking starts
		// Here you can start gesture detection with KinectManager.DetectGesture()
		void UserDetected(uint userId, int userIndex);
		
		// Invoked when a user is lost
		// Gestures for this user are cleared automatically, but you can free the used resources
		void UserLost(uint userId, int userIndex);
		
		// Invoked when a gesture is in progress 
		void GestureInProgress(uint userId, int userIndex, KinectWrapper.Gestures gesture, float progress, 
			KinectWrapper.SkeletonJoint joint, Vector3 screenPos);

		// Invoked if a gesture is completed.
		// Returns true, if the gesture detection must be restarted, false otherwise
		bool GestureCompleted(uint userId, int userIndex, KinectWrapper.Gestures gesture,
			KinectWrapper.SkeletonJoint joint, Vector3 screenPos);

		// Invoked if a gesture is cancelled.
		// Returns true, if the gesture detection must be retarted, false otherwise
		bool GestureCancelled(uint userId, int userIndex, KinectWrapper.Gestures gesture, 
			KinectWrapper.SkeletonJoint joint);
	}
	
}
