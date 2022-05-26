import { InjectionToken } from '@angular/core';
import { UserManagerSettings } from 'oidc-client';

export const AUTH_SETTINGS = new InjectionToken<UserManagerSettings>('AUTH_SETTINGS');
