import { Component, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import { APiError } from '../../../types/error';

@Component({
  imports: [],
  selector: 'app-server-error',
  styleUrl: './server-error.css',
  templateUrl: './server-error.html',
})
export class ServerError {
  private router = inject(Router);
  protected error: APiError;//= signal<APiError | null>(null);
  protected showDetails = false;

  constructor() {
    // const navigation = this.router.getCurrentNavigation();
    // this.error = navigation?.extras?.state?.['error']

    const navigation = this.router.currentNavigation();
    this.error = navigation?.extras?.state?.['error'];
  }
  detailsToggle() {
    this.showDetails = !this.showDetails;
  }
}
