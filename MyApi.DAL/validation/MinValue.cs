using System;
using System.ComponentModel.DataAnnotations;

namespace MyApi.DAL.validation;

public class MinValue : ValidationAttribute
{
    private readonly int _length;
    public MinValue(int length = 5)
    {
        _length = length;
    }
    public override bool IsValid(object? value)
    {
        if (value is int intValue)
        {
            if (intValue > _length)
            {
                return true;
            }
        }
        return false;
    }
    public override string FormatErrorMessage(string name)
    {
        return $"{name} must be greater than 5.";
    }

}
