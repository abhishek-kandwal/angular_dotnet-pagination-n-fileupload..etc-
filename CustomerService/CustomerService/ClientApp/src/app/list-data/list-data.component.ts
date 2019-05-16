import { Component, OnInit, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-list-data',
  templateUrl: './list-data.component.html',
  styleUrls: ['./list-data.component.css']
})
export class ListDataComponent {

  page = 1;
  pageSize = 10;
  currentpagesize = 0;
  public SearchData: searchdata[];

  constructor(http: HttpClient, @Inject('API_URL') apiUrl: string) {
    http.get<searchdata[]>(apiUrl + 'TestHelper/GetUsersDetails').subscribe(result => {
      this.SearchData = result;
      console.log(this.SearchData);
      this.currentpagesize = (this.SearchData.length / 10);
      console.log(this.currentpagesize);
    }, error => console.error(error));
  }

  pre() {
    if (this.page > 1) {
      this.page -= 1;
    }
  }

  next() {
    if (this.page < this.currentpagesize)
    this.page += 1;
  }
}

interface searchdata {
  Name: string;
  Address: string;
  PhoneNo: number;
  Email: string;
  State: string;
  Approved: boolean;
}
