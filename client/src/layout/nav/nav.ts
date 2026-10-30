import { Component, inject, OnInit, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../../core/services/account-service';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { ToastService } from '../../core/services/toast-service';
import { themes } from '../theme';
import { BusyService } from '../../core/services/busy-service';
import { HasRole } from '../../shared/directives/has-role';
import { finalize } from 'rxjs';

@Component({
  imports: [FormsModule, RouterLink, RouterLinkActive, HasRole],
  selector: 'app-nav',
  styleUrl: './nav.css',
  templateUrl: './nav.html',
})
export class Nav implements OnInit {

  protected accountService = inject(AccountService)
  private toast = inject(ToastService)
  protected busyService = inject(BusyService);
  protected router = inject(Router)
  protected creds: any = {};
  protected selectedTheme = signal<string>(localStorage.getItem('theme') || 'light');
  protected themes = themes; //loading theme fro themes.ts
  protected loading = signal(false);


  ngOnInit(): void {
    document.documentElement.setAttribute("data-theme", this.selectedTheme());
  }

  handleSelectTheme(theme: string) {
    this.selectedTheme.set(theme);
    localStorage.setItem("theme", theme);
    document.documentElement.setAttribute("data-theme", theme);
    const elem = document.activeElement as HTMLDivElement;
    if (elem) elem.blur();
  }

  handleSelectUserItem() {
    const elem = document.activeElement as HTMLDivElement;
    if (elem) elem.blur();
  }

  login() {

    this.loading.set(true);
    this.accountService.login(this.creds)
      .pipe(
        finalize(() => this.loading.set(false))
      )
      .subscribe({
        next: () => {

          this.router.navigateByUrl('/members');
          this.creds = { email: '', password: '' };
        }
        ,
        error: error => {

          this.toast.error(error.error);

        }

      });

  }
  logout() {
    this.accountService.logout();
    this.router.navigateByUrl('/');
  }
}
