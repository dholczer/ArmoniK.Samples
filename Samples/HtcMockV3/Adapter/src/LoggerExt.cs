﻿// This file is part of the ArmoniK project
// 
// Copyright (C) ANEO, 2021-2022.
//   W. Kirschenmann   <wkirschenmann@aneo.fr>
//   J. Gurhem         <jgurhem@aneo.fr>
//   D. Dubuc          <ddubuc@aneo.fr>
//   L. Ziane Khodja   <lzianekhodja@aneo.fr>
//   F. Lemaitre       <flemaitre@aneo.fr>
//   S. Djebbar        <sdjebbar@aneo.fr>
//   J. Fonseca        <jfonseca@aneo.fr>
//   D. Brasseur       <dbrasseur@aneo.fr>
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Disposables;
using System.Runtime.CompilerServices;

using Microsoft.Extensions.Logging;

namespace ArmoniK.Samples.HtcMock.Adapter
{
  public static class LoggerExt
  {
    public static IDisposable BeginNamedScope(this ILogger                        logger,
                                              string                              name,
                                              params ValueTuple<string, object>[] properties)
    {
      var dictionary = properties.ToDictionary(p => p.Item1,
                                               p => p.Item2);
      dictionary[name + ".Scope"] = Guid.NewGuid();
      return logger.BeginScope(dictionary);
    }

    public static IDisposable BeginPropertyScope(this   ILogger                      logger,
                                                 params ValueTuple<string, object>[] properties)
    {
      var dictionary = properties.ToDictionary(p => p.Item1,
                                               p => p.Item2);
      return logger.BeginScope(dictionary);
    }

    public static IDisposable LogFunction(this ILogger              logger,
                                          string                    id           = "",
                                          LogLevel                  level        = LogLevel.Debug,
                                          [CallerMemberName] string functionName = "")
    {
      var methodInfo = new StackTrace().GetFrame(1)?.GetMethod();
      var className  = methodInfo?.ReflectedType?.Name;

      logger.Log(level,
                 "Entering {className}.{functionName} - {id}",
                 className,
                 functionName,
                 id);

      return Disposable.Create(() => logger.Log(level,
                                                "Leaving {className}.{functionName} - {id}",
                                                className,
                                                functionName,
                                                id));
    }
  }
}