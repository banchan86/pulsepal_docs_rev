// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

var common = require('./ManagedReference.common.js');
var extension = require('./ManagedReference.extension.js');
var overwrite = require('./ManagedReference.overwrite.js');

exports.transform = function (model) {
  model.yamlmime = "ManagedReference";

  if (overwrite && overwrite.transform) {
    return overwrite.transform(model);
  }

  if (extension && extension.preTransform) {
    model = extension.preTransform(model);
  }

  if (common && common.transform) {
    model = common.transform(model);
  }


  // Convert numbers to strings in the model
  convertNumbersToStrings(model);

  if (model.type.toLowerCase() === "enum") {
    model.isClass = false;
    model.isEnum = true;
  }
  if(model._disableToc === undefined) {
    model._disableToc = model._disableToc || !model._tocPath || (model._navPath === model._tocPath);
  }
  model._disableNextArticle = true;

  if (extension && extension.postTransform) {
    if (model._splitReference) {
      model = postTransformMemberPage(model);
    }

    model = extension.postTransform(model);
  }

  return model;
}

exports.getOptions = function (model) {
  if (overwrite && overwrite.getOptions) {
    return overwrite.getOptions(model);
  }
  var ignoreChildrenBookmarks = model._splitReference && model.type && common.getCategory(model.type) === 'ns';

  return {
    "bookmarks": common.getBookmarks(model, ignoreChildrenBookmarks)
  };
}

function postTransformMemberPage(model) {
  var type = model.type.toLowerCase();
  var category = common.getCategory(type);
  if (category == 'class') {
      var typePropertyName = common.getTypePropertyName(type);
      if (typePropertyName) {
          model[typePropertyName] = true;
      }
      if (model.children && model.children.length > 0) {
          model.isCollection = true;
          common.groupChildren(model, 'class');
      } else {
          model.isItem = true;
      }
  }
  return model;
}

// Helper function to convert numbers to strings
function convertNumbersToStrings(obj) {
  if (typeof obj === 'number') {
    return obj.toString();
  } else if (Array.isArray(obj)) {
    for (var i = 0; i < obj.length; i++) {
      obj[i] = convertNumbersToStrings(obj[i]);
    }
  } else if (typeof obj === 'object' && obj !== null) {
    for (var key in obj) {
      if (obj.hasOwnProperty(key)) {
        obj[key] = convertNumbersToStrings(obj[key]);
      }
    }
  }
  return obj;
}

// debug function to verify 