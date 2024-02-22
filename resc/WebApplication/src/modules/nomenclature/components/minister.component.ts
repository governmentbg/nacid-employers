import { Component, OnInit } from '@angular/core';
import { BaseNomenclatureComponent } from '../common/base-nomenclature.component';
import { Minister } from '../models/minister.model';

@Component({
  selector: 'app-minister',
  templateUrl: './../common/base-nomenclature.component.html',
})

export class MinisterComponent extends BaseNomenclatureComponent<Minister> implements OnInit {
  ngOnInit(): void {
    this.init(Minister, 'Minister');
  }
}