import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
  title = 'Hello adiros 🤓';
  users: any; // 1. Define a property to hold the users we get back from our API

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.http.get('https://localhost:5002/api/users').subscribe({
      next: (response) => (this.users = response),
      error: (err) => console.log(err),
      complete: () => console.log('Request completed.'),
    });
  }
}
