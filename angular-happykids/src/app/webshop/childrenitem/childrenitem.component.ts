import { Component, Input, OnInit } from '@angular/core';
import { ChildrenItem } from 'src/app/shared/models/childrenitem';

@Component({
  selector: 'app-childrenitem',
  templateUrl: './childrenitem.component.html',
  styleUrls: ['./childrenitem.component.scss']
})
export class ChildrenitemComponent implements OnInit {
  @Input() childrenItem: ChildrenItem;

  constructor() { }

  ngOnInit(): void {
  }

}
