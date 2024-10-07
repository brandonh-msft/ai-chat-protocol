// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Backend.Model;
public record struct AIChatFile(string Filename, string ContentType, BinaryData Data);
