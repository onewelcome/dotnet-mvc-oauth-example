/*
 * Copyright (c) 2017 Onegini B.V.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using Microsoft.AspNetCore.Authentication;

namespace OneginiAuthClient.Onegini.AspNetCore.Authentication.OneginiBearer
{
    /// <summary>
    /// Extension method to add Onegini Bearer authentication capabilities to the ASP.NET authentication pipeline.
    /// </summary>
    public static class OneginiBearerExtensions
    {
        public static AuthenticationBuilder AddOneginiBearer(this AuthenticationBuilder builder, Action<OneginiBearerOptions> configureOptions)
        {
            return builder.AddScheme<OneginiBearerOptions, OneginiBearerHandler>(OneginiBearerOptions.DefaultScheme, configureOptions);
        }
    }
}