// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
//

// <StdHeader/>
// <Description>
// Section 7.6
// For an operation of the form -x, operator overload
// resolution is applied to select a specific operator
// implementation.  The operand is converted to the
// parameter type of the selected operator, and the
// type of the result is the return type of the operator.
// </Description>
//<Expects Status=success></Expects>

// <Code> 

using System;

public class MyClass
{

    public static int Main()
    {
        long test1 = long.MinValue;
        long test2 = 0;
        try
        {
            checked
            {
                test2 = -test1;
            }
        }
        catch (OverflowException)
        {
            return 100;
        }
        return 1;
    }
}
// </Code> 
