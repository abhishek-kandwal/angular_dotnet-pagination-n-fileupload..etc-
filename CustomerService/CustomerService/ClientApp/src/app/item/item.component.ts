import { Component,  Output, EventEmitter, Inject} from '@angular/core';
import { HttpEventType, HttpClient } from '@angular/common/http';


@Component({
  selector: 'app-item',
  templateUrl: './item.component.html',
  styleUrls: ['./item.component.css']
})

export class ItemComponent {

  public progress: number;
  public message: string;
  public fileLocation: any;
  public path: any;
  @Output() public onUploadFinished = new EventEmitter();

  public categories: any;
  public ItemMasterModel = {
    ItemId: 0,
    ItemName: "",
    CategoryID: "",
    Address:""
  };

  private _http: HttpClient;
  _baseUrl: string;

  constructor(http: HttpClient, @Inject('API_URL') apiUrl: string) {

    this._baseUrl = apiUrl;
    http.get(apiUrl + 'TestHelper/GetCategory').subscribe(result => {
      this.categories = result;
     // console.log(this.categories);
      this._http = http;
      this.ItemMasterModel = {
        ItemId: 0,
        ItemName: "",
        CategoryID: "",
        Address: ""
      }
    }, error => console.error(error));
  }


  Save() {
    this._http.post<ItemAddressModel>(this._baseUrl + 'TestHelper/SaveCategoryAddress', this.ItemMasterModel).subscribe(result => {
      alert("success");
    }, error => console.error(error));
  }

  public uploadFile = (files) => {
    if (files.length === 0) {
      return;
    }

    let fileToUpload = <File>files[0];
    const formData = new FormData();
    formData.append('file', fileToUpload);

    this._http.post(this._baseUrl + 'TestHelper/upload', formData, { reportProgress: true, observe: 'events' })
      .subscribe(event => {
        if (event.type === HttpEventType.UploadProgress)
          this.progress = Math.round(100 * event.loaded / event.total);
        else if (event.type === HttpEventType.Response) {
          this.message = 'Upload success.';
          this.onUploadFinished.emit(event.body);
          this.path = event.body['fullPath'];
        }
        this.ItemMasterModel.Address = this.path;
        console.log(this.path);
      });
  }
}

interface ItemAddressModel {
  ItemName: string;
  Address: string;
}
