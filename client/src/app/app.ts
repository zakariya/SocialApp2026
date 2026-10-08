
import { Component, inject, signal } from '@angular/core';
import { Nav } from '../layout/nav/nav';
import { Router, RouterOutlet } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-root',
  styleUrl: './app.css',
  templateUrl: './app.html',
  imports: [Nav, RouterOutlet],
})
export class App {


  protected router = inject(Router);
  private translate = inject(TranslateService);

  constructor() {
    // Register available languages
    this.translate.addLangs(['en', 'ar']);
    // Set default language
    this.translate.setDefaultLang('en');
    // Start with English
    this.translate.use('en');
  }

  switchLanguage(lang: string) {
    this.translate.use(lang);

    // Optional: flip layout direction for RTL languages
    const rtlLangs = ['ar', 'ur'];
    document.documentElement.dir = rtlLangs.includes(lang) ? 'rtl' : 'ltr';
  }






}

