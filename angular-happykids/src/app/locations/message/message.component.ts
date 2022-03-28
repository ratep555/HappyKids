import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { LocationsService } from '../locations.service';

@Component({
  selector: 'app-message',
  templateUrl: './message.component.html',
  styleUrls: ['./message.component.scss']
})
export class MessageComponent implements OnInit {
  messageForm: FormGroup;

  constructor(public locationsService: LocationsService,
              private toastr: ToastrService,
              private router: Router,
              private fb: FormBuilder) { }

  ngOnInit(): void {
    this.createMessage();
  }

  createMessage() {
    this.messageForm = this.fb.group({
      firstLastName: [null, [Validators.required,
        Validators.minLength(2), Validators.maxLength(30)]],
      messageContent: [null, [Validators.required,
        Validators.minLength(10), Validators.maxLength(2000)]],
      email: [null, [Validators.required]],
      phone: [null, [Validators.required]]
    });
  }

  onSubmit() {
    this.locationsService.createMessage(this.messageForm.value).subscribe(() => {
      this.messageForm.reset();
      this.toastr.success('Your message has been sent. We will try to contact you as soon as possible.');
    },
    error => {
      console.log(error);
    });
  }

}
