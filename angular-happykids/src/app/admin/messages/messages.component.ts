import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { ClientMessage } from 'src/app/shared/models/message';
import { UserParams } from 'src/app/shared/models/myparams';
import { MessagesService } from './messages.service';
import Swal from 'sweetalert2/dist/sweetalert2.js';
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.scss']
})
export class MessagesComponent implements OnInit {
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  messages: ClientMessage[];
  userParams: UserParams;
  totalCount: number;

  sortOptions = [
    {name: 'All', value: 'all'},
    {name: 'Answered', value: 'answered'},
    {name: 'Unanswered', value: 'unanswered'}
  ];

  constructor(private messagesService: MessagesService,
              private toastr: ToastrService,
              private  router: Router) {
              this.userParams = this.messagesService.getUserParams();
               }

  ngOnInit(): void {
    this.getAllMessages();
  }

  getAllMessages() {
    this.messagesService.setUserParams(this.userParams);
    this.messagesService.getMessages(this.userParams)
    .subscribe(response => {
      this.messages = response.data;
      this.userParams.page = response.page;
      this.userParams.pageCount = response.pageCount;
      this.totalCount = response.count;
    }, error => {
      console.log(error);
    }
    );
  }

  onSortSelected(sort: string) {
      this.userParams.sort = sort;
      this.getAllMessages();
    }

  onSearch() {
    this.userParams.query = this.searchTerm.nativeElement.value;
    this.getAllMessages();
  }

  onReset() {
    this.searchTerm.nativeElement.value = '';
    this.userParams = this.messagesService.resetUserParams();
    this.getAllMessages();
  }

  onPageChanged(event: any) {
    if (this.userParams.page !== event) {
      this.userParams.page = event;
      this.messagesService.setUserParams(this.userParams);
      this.getAllMessages();
    }
}

onDelete(id: number) {
  Swal.fire({
    title: 'Are you sure want to delete this record?',
    text: 'You will not be able to recover it afterwards!',
    icon: 'warning',
    showCancelButton: true,
    confirmButtonText: 'Yes, delete it!',
    confirmButtonColor: '#DD6B55',
    cancelButtonText: 'No, keep it'
  }).then((result) => {
    if (result.value) {
        this.messagesService.deleteMessage(id)
    .subscribe(
      res => {
        this.getAllMessages();
        this.toastr.error('Deleted successfully!');
      }, err => { console.log(err);
       });
  }
});
}

}
