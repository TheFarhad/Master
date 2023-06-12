﻿namespace Master.Endpoint.ModelBinding;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

public sealed class NonValidatingValidator : IObjectModelValidator
{
    public void Validate(ActionContext actionContext, ValidationStateDictionary validationState, string prefix, object model)
    {
        foreach (var entry in actionContext.ModelState.Values)
        {
            entry.ValidationState = ModelValidationState.Skipped;
        }
    }
}