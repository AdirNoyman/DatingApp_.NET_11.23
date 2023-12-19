import { Component } from '@angular/core';
import { AccountService } from '../services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css'],
})
export class NavComponent {
  model: any = {};

  isLoggedIn: boolean = false;

  // Inject the Account service into the constructor
  constructor(private accountService: AccountService) {}

  ngOnInit(): void {
    this.getCurrentUser();
    console.log('User is logged in?', this.isLoggedIn);
  }

  getCurrentUser() {
    // Subscribe to the currentUser$ observable
    this.accountService.currentUser$.subscribe({
      next: (user) => (this.isLoggedIn = !!user), // !!user => if user is null or undefined, return false, else return true
      error: (error) => console.log('Error in getCurrentUser() 🫣 => ', error),
    });
  }

  login() {
    this.accountService.login(this.model).subscribe({
      // Success login
      next: (response) => {
        console.log(response);
        this.isLoggedIn = true;
      },
      // Failure login
      error: (error) => console.log('Error in Login 🫣 => ', error),
    });
  }

  logout() {
    this.accountService.logout();
    this.isLoggedIn = false;
  }
}
