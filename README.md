[![Community Extension](https://img.shields.io/badge/Community%20Extension-An%20open%20source%20community%20maintained%20project-FF4700)](https://github.com/camunda-community-hub/community)
![Compatible with: Camunda Platform 8](https://img.shields.io/badge/Compatible%20with-Camunda%20Platform%208-0072Ce)
[![](https://img.shields.io/badge/Lifecycle-Incubating-blue)](https://github.com/Camunda-Community-Hub/community/blob/main/extension-lifecycle.md#incubating-)

# Camunda 8 DotNet low-code tasklist

This project is designed to show how to use [zb-client](https://github.com/camunda-community-hub/zeebe-client-csharp) with net6.0. Front-end is written in React and rely on FormIO and FormJs. This project is a draft. 
For now, this application only runs against Camunda 8 SaaS. It should not be too complex to make it available to Camunda 8 SM but that's not my priority now. Feel free to propose PR if you want to :)

This project is a fork of https://github.com/camunda-community-hub/dotnet-custom-tasklist

This ReadMe should be improved...

## Mails

Mail templating is built on top of Stubble (Mustache implementation) and mail sending is achieved through Gmail. You should then add a client_secrets.json to you project : https://developers.google.com/api-client-library/dotnet/guide/aaa_oauth?hl=fr

