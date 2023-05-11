import React, { useState, useEffect } from 'react';
import FormJsViewer from './FormJsViewer';
import FormIoViewer from './FormIoViewer';
import formService from '../service/FormService'
import { IFormViewer } from '../store/model';
import { Component, FC } from 'react';

function FormResolver(formViewer: IFormViewer) {
  const FormFinder: FC<IFormViewer> = formViewer.schema && formViewer.schema.generator=='formIo' ? FormIoViewer : formService.customFormExists(formViewer.formKey) ? formService.getForm(formViewer.formKey)! : FormJsViewer;
  
  let variables:any = undefined;
  
    if (formViewer.variables) {
      variables = {};
      for (let i=0;i<formViewer.variables.length;i++) {
		let variable = formViewer.variables[i];
        variables[variable.name]= JSON.parse(variable.value);
      }
    }
  
  return (<FormFinder formKey={formViewer.formKey} schema={formViewer.schema} variables={variables} disabled={formViewer.disabled}></FormFinder>)
}

export default FormResolver;
