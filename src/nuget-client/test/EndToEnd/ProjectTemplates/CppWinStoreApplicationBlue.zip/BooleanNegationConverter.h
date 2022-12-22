#pragma once

namespace $safeprojectname$
{
	namespace Common
	{
		/// <summary>
		/// Value converter that translates true to false and vice versa.
		/// </summary>
		[Windows::Foundation::Metadata::WebHostHidden]
		public ref class BooleanNegationConverter sealed : Windows::UI::Xaml::Data::IValueConverter
		{
		public:
			virtual Platform::Object^ Convert(Platform::Object^ value, Windows::UI::Xaml::Interop::TypeName targetType, Platform::Object^ parameter, Platform::String^ language);
			virtual Platform::Object^ ConvertBack(Platform::Object^ value, Windows::UI::Xaml::Interop::TypeName targetType, Platform::Object^ parameter, Platform::String^ language);
		};
	}
}
