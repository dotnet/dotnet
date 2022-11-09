#pragma once

#include <collection.h>

namespace $safeprojectname$
{
	namespace Common
	{
		/// <summary>
		/// Wrapper for <see cref="RichTextBlock"/> that creates as many additional overflow
		/// columns as needed to fit the available content.
		/// </summary>
		/// <example>
		/// The following creates a collection of 400-pixel wide columns spaced 50 pixels apart
		/// to contain arbitrary data-bound content:
		/// <code>
		/// <RichTextColumns>
		///     <RichTextColumns.ColumnTemplate>
		///         <DataTemplate>
		///             <RichTextBlockOverflow Width="400" Margin="50,0,0,0"/>
		///         </DataTemplate>
		///     </RichTextColumns.ColumnTemplate>
		///     
		///     <RichTextBlock Width="400">
		///         <Paragraph>
		///             <Run Text="{Binding Content}"/>
		///         </Paragraph>
		///     </RichTextBlock>
		/// </RichTextColumns>
		/// </code>
		/// </example>
		/// <remarks>Typically used in a horizontally scrolling region where an unbounded amount of
		/// space allows for all needed columns to be created.  When used in a vertically scrolling
		/// space there will never be any additional columns.</remarks>
		[Windows::UI::Xaml::Markup::ContentProperty(Name = "RichTextContent")]
		[Windows::Foundation::Metadata::WebHostHidden]
		public ref class RichTextColumns sealed : Windows::UI::Xaml::Controls::Panel
		{
		public:
			RichTextColumns();
			static property Windows::UI::Xaml::DependencyProperty^ RichTextContentProperty
			{
				Windows::UI::Xaml::DependencyProperty^ get();
			};
			static property Windows::UI::Xaml::DependencyProperty^ ColumnTemplateProperty
			{
				Windows::UI::Xaml::DependencyProperty^ get();
			}
			property Windows::UI::Xaml::DataTemplate^ ColumnTemplate
			{
				Windows::UI::Xaml::DataTemplate^ get() { return safe_cast<Windows::UI::Xaml::DataTemplate^>(GetValue(ColumnTemplateProperty)); }
				void set(Windows::UI::Xaml::DataTemplate^ value) { SetValue(ColumnTemplateProperty, value); }
			};
			property Windows::UI::Xaml::Controls::RichTextBlock^ RichTextContent
			{
				Windows::UI::Xaml::Controls::RichTextBlock^ get() { return safe_cast<Windows::UI::Xaml::Controls::RichTextBlock^>(GetValue(RichTextContentProperty)); }
				void set(Windows::UI::Xaml::Controls::RichTextBlock^ value) { SetValue(RichTextContentProperty, value); }
			};

		protected:
			virtual Windows::Foundation::Size MeasureOverride(Windows::Foundation::Size availableSize) override;
			virtual Windows::Foundation::Size ArrangeOverride(Windows::Foundation::Size finalSize) override;

		internal:
			static void ResetOverflowLayout(Windows::UI::Xaml::DependencyObject^ d, Windows::UI::Xaml::DependencyPropertyChangedEventArgs^ e);

		private:
			Platform::Collections::Vector<Windows::UI::Xaml::Controls::RichTextBlockOverflow^>^ _overflowColumns;
		};
	}
}
