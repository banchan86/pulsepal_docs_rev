// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

/**
 * This method will be called at the start of exports.transform in ManagedReference.html.primary.js
 */
exports.preTransform = function (model) {
  if (model.inheritance){
    let operatorType = {"source": false, "sink": false, "combinator": false};
    inheritanceLength = model.inheritance.length;
    for (let i = 0; i < inheritanceLength; i++){
      if (model.inheritance[i].uid.includes('Bonsai.Source')){
        operatorType.source = true;
      }
      else if (model.inheritance[i].uid.includes('Bonsai.Sink')) {
        operatorType.sink = true;
      }
      else if (model.inheritance[i].uid.includes('Bonsai.Combinator')) {
        operatorType.combinator = true;
      }
    }
    if (!(operatorType.source || operatorType.sink || operatorType.combinator)){
      if (model.syntax){
        if (model.syntax.content[0].value.includes("[WorkflowElementCategory(ElementCategory.Source)]")){
          operatorType.source = true;
        }
        else if (model.syntax.content[0].value.includes("[WorkflowElementCategory(ElementCategory.Sink)]")){
          operatorType.sink = true;
        }
        else if (model.syntax.content[0].value.includes("[WorkflowElementCategory(ElementCategory.Combinator)]")){
          operatorType.combinator = true;
        }
      } 
    }
    if (operatorType.source){
      model.sourceNode = true;
    }
    else if (operatorType.sink){
      model.sinkNode = true;
    }
    else if (operatorType.combinator){
      model.combinatorNode = true;
    }
  }
  return model;
}

/**
 * This method will be called at the end of exports.transform in ManagedReference.html.primary.js
 */
exports.postTransform = function (model) {
  return model;
}
