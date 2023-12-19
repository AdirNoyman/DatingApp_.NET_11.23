import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AccountService } from './services/account.service';
import { User } from './models/user';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
  title = 'Hello adiros 🤓';
  users: any; // 1. Define a property to hold the users we get back from our API

  constructor(
    private http: HttpClient,
    private accountService: AccountService
  ) {}

  ngOnInit(): void {
    this.getUsers();
    this.setCurrentUser();
  }

  getUsers() {
    this.http.get('https://localhost:5002/api/users').subscribe({
      next: (response) => (this.users = response),
      error: (err) => console.log(err),
      complete: () => console.log('Request completed.'),
    });
  }

  setCurrentUser() {
    const userString = localStorage.getItem('user');
    if (!userString) return; // If the userString is null or undefined, return  (this will happen if the user is not logged in)
    const user: User = JSON.parse(userString); // Parse the userString to a User JS object
    this.accountService.setCurrentUser(user); // Update the currentUserSource BehaviorSubject with the user object
  }
}
