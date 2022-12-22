//
// SuspensionManager.h
// Declaration of the SuspensionManager class
//

#pragma once

namespace $safeprojectname$
{
	namespace Common
	{
		/// <summary>
		/// SuspensionManager captures global session state to simplify process lifetime management
		/// for an application.  Note that session state will be automatically cleared under a variety
		/// of conditions and should only be used to store information that would be convenient to
		/// carry across sessions, but that should be disacarded when an application crashes or is
		/// upgraded.
		/// </summary>
		class SuspensionManager sealed
		{
		public:
			static void RegisterFrame(Windows::UI::Xaml::Controls::Frame^ frame, Platform::String^ sessionStateKey);
			static void UnregisterFrame(Windows::UI::Xaml::Controls::Frame^ frame);
			static concurrency::task<void> SaveAsync();
			static concurrency::task<void> RestoreAsync();
			static Windows::Foundation::Collections::IMap<Platform::String^, Platform::Object^>^ SessionState();
			static Windows::Foundation::Collections::IMap<Platform::String^, Platform::Object^>^ SessionStateForFrame(
				Windows::UI::Xaml::Controls::Frame^ frame);

		private:
			static void RestoreFrameNavigationState(Windows::UI::Xaml::Controls::Frame^ frame);
			static void SaveFrameNavigationState(Windows::UI::Xaml::Controls::Frame^ frame);

			static Platform::Collections::Map<Platform::String^, Platform::Object^>^ _sessionState;
			static const wchar_t* sessionStateFilename;

			static std::vector<Platform::WeakReference> _registeredFrames;
			static Windows::UI::Xaml::DependencyProperty^ FrameSessionStateKeyProperty;
			static Windows::UI::Xaml::DependencyProperty^ FrameSessionStateProperty;
		};
	}
}
