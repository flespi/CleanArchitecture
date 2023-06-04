import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/app.module';
import { environment } from './environments/environment';

import { APP_SETTINGS } from './app/app.settings';
import { settings } from './settings/settings';

if (environment.production) {
  enableProdMode();
}

const settingsUrl = '/assets/settings.json';

const loadSettings = environment.production
  ? fetch(settingsUrl).then(res => res.json())
  : Promise.resolve(settings)

loadSettings.then(settings => {
  const providers = [
    { provide: APP_SETTINGS, useValue: settings }
  ];

  platformBrowserDynamic(providers).bootstrapModule(AppModule)
  .catch(err => console.log(err));
});
