import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../../core/services/account-service';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { ToastService } from '../../core/services/toast-service';

@Component({
  imports: [FormsModule, RouterLink, RouterLinkActive],
  selector: 'app-nav',
  styleUrl: './nav.css',
  templateUrl: './nav.html',
})
export class Nav {
  protected accountService = inject(AccountService)
  private toast = inject(ToastService)
  protected router = inject(Router)
  protected creds: any = {}


  login() {


    this.accountService.login(this.creds).subscribe({
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
