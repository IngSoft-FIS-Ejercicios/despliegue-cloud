import { Component, OnInit, Input } from '@angular/core';
import { Pharmacy } from '../../interfaces/pharmacy';
import { CommonService } from '../../services/CommonService';
import { PharmacyService } from '../../services/pharmacy.service';
import { DrugService } from '../../services/drug.service';
import { Router } from '@angular/router';
import { cilUser, cilAccountLogout } from '@coreui/icons';

@Component({
  selector: 'app-custom-header',
  templateUrl: './custom-header.component.html',
  styleUrls: ['./custom-header.component.css'],
})
export class CustomHeaderComponent implements OnInit {
  pharmacys: Pharmacy[] = [];
  badge: number = 0;
  searchValue: string = '';
  pharmacyId: string = '0';
  icons = { cilUser, cilAccountLogout };

  @Input() cartAndTrack: boolean = true;
  @Input() select: boolean = true;
  @Input() search: boolean = true;
  @Input() username: string = '';

  constructor(
    private commonService: CommonService,
    private pharmacyService: PharmacyService,
    private drugService: DrugService,
    private router: Router,
  ) {
    this.commonService.onHeaderDataUpdate.subscribe((data: number) => {
      this.badge = data;
    });
  }

  ngOnInit(): void {
    this.getPharmacys();
  }

  getPharmacys(): void {
    this.pharmacyService
      .getPharmacys()
      .subscribe((pharmacys) => (this.pharmacys = pharmacys));
  }

  onChangeSelect(id: string): void {
    this.pharmacyId = id;
    this.getDrugsFilter(this.pharmacyId, this.searchValue);
  }

  onSearch() {
    this.getDrugsFilter(this.pharmacyId, this.searchValue);
  }

  onLogin() {
    this.router.navigate(['/login']);
  }

  logout(): void {
    this.router.navigate(['login']);
  }

  getDrugsFilter(pharmacyId: string, pharmacyName: string): void {
    this.drugService
      .getDrugsFilter(pharmacyId, pharmacyName)
      .subscribe((drugs) => {
        this.commonService.updateSearchData(drugs);
      });
  }
}
