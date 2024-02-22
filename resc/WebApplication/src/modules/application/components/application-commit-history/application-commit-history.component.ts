import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommitStateEnumLocalization } from 'src/modules/enum-localization.const';
import { ApplicationLotHistoryDto } from '../../models/application-lot-history.dto';
import { CommitState } from 'src/infrastructure/enums/commit-state.enum';

@Component({
  selector: 'app-application-commit-history',
  templateUrl: './application-commit-history.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ApplicationCommitHistoryComponent implements OnInit {
  model: ApplicationLotHistoryDto;
  commitStates = CommitState;
  enumLocalization = CommitStateEnumLocalization;
  lotId: number;

  constructor(
    private activatedRoute: ActivatedRoute,
    private router: Router,
  ) { }

  ngOnInit(): void {
    this.activatedRoute.data
      .subscribe((data: { model: ApplicationLotHistoryDto }) => this.model = data.model);

    this.lotId = +this.activatedRoute.snapshot.paramMap.get('lotId');
  }

  backClicked() {
    this.router.navigate(['/application', 'lot', this.lotId, 'commit', this.model.actualCommitId]);
  }
}
