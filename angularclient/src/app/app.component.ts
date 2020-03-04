import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  time: string;

  constructor(private client: HttpClient) {}

  updateTime() {
    this.client.get<{data: string}>('http://localhost:1337/time')
    .subscribe(r => {
      this.time = r.data;
    });
  }
}
