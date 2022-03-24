import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BranchesService } from 'src/app/admin/branches/branches.service';
import { Branch } from 'src/app/shared/models/branch';
import { CoordinatesMapWithMessage } from 'src/app/shared/models/coordinate';

@Component({
  selector: 'app-location-detail',
  templateUrl: './location-detail.component.html',
  styleUrls: ['./location-detail.component.scss']
})
export class LocationDetailComponent implements OnInit {
  location: Branch;
  coordinates: CoordinatesMapWithMessage[] = [];


  constructor(private branchesService: BranchesService,
              private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe((params) => {
      this.branchesService.getBranchById(params.id).subscribe((location) => {
        console.log(location);
        this.location = location;
        this.coordinates = [{latitude: location.latitude, longitude: location.longitude, message: location.street}];
        console.log(this.coordinates);
      });
    });
  }

}
