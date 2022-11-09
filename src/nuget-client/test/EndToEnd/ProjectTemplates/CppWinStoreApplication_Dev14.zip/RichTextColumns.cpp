#include "pch.h"
#include "RichTextColumns.h"

using namespace $safeprojectname$::Common;

using namespace Platform;
using namespace Platform::Collections;
using namespace Windows::Foundation;
using namespace Windows::UI::Xaml;
using namespace Windows::UI::Xaml::Controls;
using namespace Windows::UI::Xaml::Interop;

/// <summary>
/// Initializes a new instance of the <see cref="RichTextColumns"/> class.
/// </summary>
RichTextColumns::RichTextColumns()
{
	HorizontalAlignment = Windows::UI::Xaml::HorizontalAlignment::Left;
}

static DependencyProperty^ _columnTemplateProperty =
	DependencyProperty::Register("ColumnTemplate", TypeName(DataTemplate::typeid), TypeName(RichTextColumns::typeid),
	ref new PropertyMetadata(nullptr, ref new PropertyChangedCallback(
	&RichTextColumns::ResetOverflowLayout)));

/// <summary>
/// Identifies the <see cref="ColumnTemplate"/> dependency property.
/// </summary>
DependencyProperty^ RichTextColumns::ColumnTemplateProperty::get()
{
	return _columnTemplateProperty;
}

static DependencyProperty^ _richTextContentProperty =
	DependencyProperty::Register("RichTextContent", TypeName(RichTextBlock::typeid), TypeName(RichTextColumns::typeid),
	ref new PropertyMetadata(nullptr, ref new PropertyChangedCallback(
	&RichTextColumns::ResetOverflowLayout)));

/// <summary>
/// Identifies the <see cref="RichTextContent"/> dependency property.
/// </summary>
DependencyProperty^ RichTextColumns::RichTextContentProperty::get()
{
	return _richTextContentProperty;
}

/// <summary>
/// Invoked when the content or overflow template is changed to recreate the column layout.
/// </summary>
/// <param name="d">Instance of <see cref="RichTextColumns"/> where the change
/// occurred.</param>
/// <param name="e">Event data describing the specific change.</param>
void RichTextColumns::ResetOverflowLayout(DependencyObject^ d, DependencyPropertyChangedEventArgs^ e)
{
	(void) e;	// Unused parameter

	auto target = dynamic_cast<RichTextColumns^>(d);
	if (target != nullptr)
	{
		// When dramatic changes occur, rebuild layout from scratch
		target->_overflowColumns = nullptr;
		target->Children->Clear();
		target->InvalidateMeasure();
	}
}

/// <summary>
/// Determines whether additional overflow columns are needed and if existing columns can
/// be removed.
/// </summary>
/// <param name="availableSize">The size of the space available, used to constrain the
/// number of additional columns that can be created.</param>
/// <returns>The resulting size of the original content plus any extra columns.</returns>
Size RichTextColumns::MeasureOverride(Size availableSize)
{
	if (RichTextContent == nullptr)
	{
		Size emptySize(0, 0);
		return emptySize;
	}

	// Make sure the RichTextBlock is a child, using the lack of
	// a list of additional columns as a sign that this hasn't been
	// done yet
	if (_overflowColumns == nullptr)
	{
		Children->Append(RichTextContent);
		_overflowColumns = ref new Vector<RichTextBlockOverflow^>();
	}

	// Start by measuring the original RichTextBlock content
	RichTextContent->Measure(availableSize);
	auto maxWidth = RichTextContent->DesiredSize.Width;
	auto maxHeight = RichTextContent->DesiredSize.Height;
	auto hasOverflow = RichTextContent->HasOverflowContent;

	// Make sure there are enough overflow columns
	unsigned int overflowIndex = 0;
	while (hasOverflow && maxWidth < availableSize.Width && ColumnTemplate != nullptr)
	{
		// Use existing overflow columns until we run out, then create
		// more from the supplied template
		RichTextBlockOverflow^ overflow;
		if (_overflowColumns->Size > overflowIndex)
		{
			overflow = _overflowColumns->GetAt(overflowIndex);
		}
		else
		{
			overflow = safe_cast<RichTextBlockOverflow^>(ColumnTemplate->LoadContent());
			_overflowColumns->Append(overflow);
			Children->Append(overflow);
			if (overflowIndex == 0)
			{
				RichTextContent->OverflowContentTarget = overflow;
			}
			else
			{
				_overflowColumns->GetAt(overflowIndex - 1)->OverflowContentTarget = overflow;
			}
		}

		// Measure the new column and prepare to repeat as necessary
		Size remainingSize(availableSize.Width - maxWidth, availableSize.Height);
		overflow->Measure(remainingSize);
		maxWidth += overflow->DesiredSize.Width;
		maxHeight = __max(maxHeight, overflow->DesiredSize.Height);
		hasOverflow = overflow->HasOverflowContent;
		overflowIndex++;
	}

	// Disconnect extra columns from the overflow chain, remove them from our private list
	// of columns, and remove them as children
	if (_overflowColumns->Size > overflowIndex)
	{
		if (overflowIndex == 0)
		{
			RichTextContent->OverflowContentTarget = nullptr;
		}
		else
		{
			_overflowColumns->GetAt(overflowIndex - 1)->OverflowContentTarget = nullptr;
		}
		while (_overflowColumns->Size > overflowIndex)
		{
			_overflowColumns->RemoveAt(overflowIndex);
			Children->RemoveAt(overflowIndex + 1);
		}
	}

	// Report final determined size
	Size resultingSize(maxWidth, maxHeight);
	return resultingSize;
}

/// <summary>
/// Arranges the original content and all extra columns.
/// </summary>
/// <param name="finalSize">Defines the size of the area the children must be arranged
/// within.</param>
/// <returns>The size of the area the children actually required.</returns>
Size RichTextColumns::ArrangeOverride(Size finalSize)
{
	float maxWidth = 0;
	float maxHeight = 0;
	auto childrenIterator = Children->First();
	while (childrenIterator->HasCurrent)
	{
		auto child = childrenIterator->Current;
		Rect childRect(maxWidth, 0, child->DesiredSize.Width, finalSize.Height);
		child->Arrange(childRect);
		maxWidth += child->DesiredSize.Width;
		maxHeight = __max(maxHeight, child->DesiredSize.Height);
		childrenIterator->MoveNext();
	}
	Size resultingSize(maxWidth, maxHeight);
	return resultingSize;
}
