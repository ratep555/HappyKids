import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../account.service';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.scss']
})
export class ResetPasswordComponent implements OnInit {
  resetPasswordForm: FormGroup;
  token: string;
  email: string;

  constructor(private accountService: AccountService,
              private fb: FormBuilder,
              private route: ActivatedRoute,
              private toastr: ToastrService) { }

  ngOnInit(): void {
    this.token = this.route.snapshot.queryParamMap.get('token');
    this.email = this.route.snapshot.queryParamMap.get('email');

    this.resetPasswordForm = this.fb.group({
      password: ['', Validators.required],
      confirmPassword: ['', [Validators.required, this.compareValues('password')]],
      email: [this.email],
      token: [this.token],
    });
  }

  onSubmit() {
    this.accountService.resetPassword(this.resetPasswordForm.value).subscribe(() => {
      this.toastr.success('Password updated successfully. Feel free to login at any time.');
    },
     error => {
      console.log(error);
    }
    );
  }

  compareValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control?.value === control?.parent?.controls[matchTo].value
        ? null : {isEqual: true};
    };
  }

}
