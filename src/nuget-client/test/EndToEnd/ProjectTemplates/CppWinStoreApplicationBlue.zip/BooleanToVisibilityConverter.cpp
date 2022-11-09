#include "pch.h"
#include "BooleanToVisibilityConverter.h"

using namespace $safeprojectname$::Common;

using namespace Platform;
using namespace Windows::Foundation;
using namespace Windows::UI::Xaml;
using namespace Windows::UI::Xaml::Interop;

Object^ BooleanToVisibilityConverter::Convert(Object^ value, TypeName targetType, Object^ parameter, String^ language)
{
	(void) targetType;	// Unused parameter
	(void) parameter;	// Unused parameter
	(void) language;	// Unused parameter

	auto boxedBool = dynamic_cast<Box<bool>^>(value);
	auto boolValue = (boxedBool != nullptr && boxedBool->Value);
	return (boolValue ? Visibility::Visible : Visibility::Collapsed);
}

Object^ BooleanToVisibilityConverter::ConvertBack(Object^ value, TypeName targetType, Object^ parameter, String^ language)
{
	(void) targetType;	// Unused parameter
	(void) parameter;	// Unused parameter
	(void) language;	// Unused parameter

	auto visibility = dynamic_cast<Box<Visibility>^>(value);
	return (visibility != nullptr && visibility->Value == Visibility::Visible);
}
