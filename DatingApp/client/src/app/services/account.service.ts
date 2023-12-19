import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs';
import { User } from '../models/user';

@Injectable({
  // Provided(scoped) in the app's root module (meaning available to all the apps components). Has the scope of Singelton (will live along the lifetime of the application)
  providedIn: 'root',
})
export class AccountService {
  baseUrl = 'https://localhost:5002/api/';
  private currentUserSource = new BehaviorSubject<User | null>(null); // BehaviorSubject is a type of Subject that allows us to set the initial value of the subject. It also requires us to provide an initial value when we instantiate the class. We set the initial value to null.
  currentUser$ = this.currentUserSource.asObservable(); // $ is a naming convention for observables

  constructor(private http: HttpClient) {}

  login(model: any) {
    return this.http.post<User>(this.baseUrl + 'account/login', model).pipe(
      // The pipe operator allows us to chain multiple rxjs operators together
      map((response: User) => {
        const user = response;
        if (user) {
          // save the user object in the user's browser local storage
          localStorage.setItem('user', JSON.stringify(user)); // Store
          this.currentUserSource.next(user); // Update the currentUserSource BehaviorSubject with the user object
        }
      })
    );
  }

  setCurrentUser(user: User) {
    this.currentUserSource.next(user); // Update the currentUserSource BehaviorSubject with the user object
  }

  logout() {
    localStorage.removeItem('user'); // Remove the user object from the user's browser local storage
    this.currentUserSource.next(null); // Update the currentUserSource BehaviorSubject with null
  }
}
