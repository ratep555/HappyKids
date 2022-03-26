import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MessageEdit } from 'src/app/shared/models/message';
import { MessagesService } from '../messages.service';

@Component({
  selector: 'app-edit-message',
  templateUrl: './edit-message.component.html',
  styleUrls: ['./edit-message.component.scss']
})
export class EditMessageComponent implements OnInit {
  messageForms: FormArray = this.fb.array([]);
  id: number;

  constructor(private messagesService: MessagesService,
              private fb: FormBuilder,
              private activatedRoute: ActivatedRoute,
              private router: Router) { }

  ngOnInit(): void {
    this.id = this.activatedRoute.snapshot.params['id'];

    this.messagesService.getMessageById(this.id).subscribe(
      (message: MessageEdit) => {
      this.messageForms.push(this.fb.group({
        id: [this.id],
        firstLastName: [message.firstLastName],
        messageContent: [message.messageContent],
        email: [message.email],
        phone: [message.phone],
        isReplied: [message.isReplied],
        sendingDate: [new Date(message.sendingDate)]
        }));
      });
  }

  recordSubmit(fg: FormGroup) {
    this.messagesService.updateMessage(fg.value).subscribe(
      (res: any) => {
        this.router.navigateByUrl('messages');
      }, error => {
        console.log(error);
    });
  }

}
