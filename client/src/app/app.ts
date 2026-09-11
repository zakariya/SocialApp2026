import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit, signal } from '@angular/core';
import { lastValueFrom } from 'rxjs';

@Component({
  selector: 'app-root',
  styleUrl: './app.css',
  templateUrl: './app.html',
})
export class App implements OnInit {

  private http = inject(HttpClient);
  protected readonly title = signal('Social App');
  protected members = signal<any>([]);

  async ngOnInit() {

    this.members.set(await this.getMembers());

    /*
    this.http.get('https://localhost:5201/api/members').subscribe({
      next: response => this.members.set(response),
      error: error => console.log(error),
      complete: () => console.log('Completed the http request')
    })
    */
  }

  async getMembers() {
    try {
      return lastValueFrom(this.http.get('https://localhost:5201/api/members'));

    } catch (error) {
      console.log(error);
      throw error;
    }
  }
}
