import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/app.module';
import { environment } from './environments/environment';

export function getBaseUrl() {
  return document.getElementsByTagName('base')[0].href;
}

export function getBaseUrlData() {
  return "http://localhost:58842/api/";
}

const providers = [
  { provide: 'API_URL', useFactory: getBaseUrlData, deps: [] },
  { provide: 'BASE_URL', useFactory: getBaseUrl, deps: [] }

];

if (environment.production) {
  enableProdMode();
}

platformBrowserDynamic(providers).bootstrapModule(AppModule)
  .catch(err => console.log(err));
