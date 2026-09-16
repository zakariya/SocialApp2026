import { Component, inject, input, output, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RegisterCreds, User } from '../../../types/user';
import { AccountService } from '../../../core/services/account-service';

@Component({
  imports: [FormsModule],
  selector: 'app-register',
  styleUrl: './register.css',
  templateUrl: './register.html',
})
export class Register {

  // public membersFromHome = input.required<User[]>();
  protected creds = {} as RegisterCreds;
  cancelRegister = output<boolean>();
  private accountService = inject(AccountService);

  register() {
    this.accountService.register(this.creds).subscribe({
      next: response => {
        console.log(response);
        this.cancel();
      }, error: error => {
        console.log(error);
        alert(error.message);

      }

    })
  }
  cancel() {
    this.cancelRegister.emit(false);
    console.log("cancelled!!!")
  }


}
