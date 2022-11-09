#include "stdafx.h"
#include "CppUnitTest.h"

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace CPPSimpleProj
{		
	TEST_CLASS(UnitTest1)
	{
	public:
		
		TEST_METHOD(PassingTest)
		{
			Assert::AreEqual(2, 2);
		}

		TEST_METHOD(FailingTest)
		{
			Assert::AreEqual(1, 2);
		}

	};
}