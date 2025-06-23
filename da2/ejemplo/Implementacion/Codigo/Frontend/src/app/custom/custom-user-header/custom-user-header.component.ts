import { Component, Input } from '@angular/core';
import { cilAccountLogout, cilMenu, cilCog } from '@coreui/icons';
import { IconSetService } from '@coreui/icons-angular';
import { ClassToggleService, HeaderComponent } from '@coreui/angular';
import { Router } from '@angular/router';
import { OnInit } from '@angular/core';
import { StorageManager } from 'src/app/utils/storage-manager';

@Component({
  selector: 'app-user-header',
  templateUrl: './custom-user-header.component.html',
})
export class UserHeaderComponent extends HeaderComponent implements OnInit {
  username: string = '';

  @Input() sidebarId: string = 'sidebar';
  @Input() title: string = '';
  @Input() link: string = '';

  constructor(
    private classToggler: ClassToggleService,
    private router: Router,
    public iconSet: IconSetService,
    private storageManager: StorageManager,
  ) {
    iconSet.icons = { cilAccountLogout, cilMenu, cilCog };
    super();
  }

  ngOnInit(): void {
    const loginData = this.storageManager.getData('login');
    if (loginData) {
      const parsedData = JSON.parse(loginData);
      this.username = parsedData?.userName || '';
    }
  }

  logout(): void {
    this.router.navigate(['login']);
  }
}
