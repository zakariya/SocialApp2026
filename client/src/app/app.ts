
import { Component, inject, signal } from '@angular/core';
import { Nav } from '../layout/nav/nav';
import { Router, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  styleUrl: './app.css',
  templateUrl: './app.html',
  imports: [Nav, RouterOutlet],
})
export class App {


  protected router = inject(Router);







}
