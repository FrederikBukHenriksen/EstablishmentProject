import { Injectable, inject } from '@angular/core';
import {
  AnalysisClient,
  ClusteringCommand,
  ClusteringReturn,
  Clustering_TimeOfVisit_TotalPrice_Command,
  EstablishmentClient,
  EstablishmentDTO,
  SaleClient,
  SaleDTO,
} from 'api';
import { lastValueFrom } from 'rxjs';
import { SessionStorageService } from '../session-storage/session-storage.service';
import { SaleService } from './sale.service';

export type Clusters = {
  clusters: Cluster[];
  calculationValues: { entityId: string; values: number[] }[];
};

export type Cluster = {
  clusterId: number;
  cluster: string[];
};

@Injectable({
  providedIn: 'root',
})
export class ClusterService {
  private readonly analysisClient = inject(AnalysisClient);
  private readonly saleService = inject(SaleService);

  private readonly sessionStorageService = inject(SessionStorageService);
  private readonly activeEstablishment =
    this.sessionStorageService.getActiveEstablishment();

  public async Cluster_TimeOfVisit_vs_TotalSpend(
    salesIds: string[],
    bandwidthTimeOfVisit: number,
    bandwidthTotalPrice: number,
    establishmentId?: string
  ): Promise<ClusteringReturn> {
    var command = new Clustering_TimeOfVisit_TotalPrice_Command();
    command.salesIds = salesIds;
    command.establishmentId = establishmentId ?? this.activeEstablishment ?? '';
    command.bandwidthTimeOfVisit = bandwidthTimeOfVisit;
    command.bandwidthTotalPrice = bandwidthTotalPrice;

    return await lastValueFrom(
      this.analysisClient.timeOfVisitTotalPrice(command)
    );
  }

  public async SaleIdCluster_to_SaleDTOCluster(
    clusterReturn: ClusteringReturn
  ): Promise<SaleDTO[][]> {
    var saleDTO = await this.saleService.getSalesDTO(
      clusterReturn.clusters.flatMap((cluster) => cluster)
    );
    return this.ClustersMatchSaleIdsAndSaleDTOs(
      clusterReturn.clusters,
      saleDTO
    );
  }

  private ClustersMatchSaleIdsAndSaleDTOs(
    id: string[][],
    sales: SaleDTO[]
  ): SaleDTO[][] {
    return id.map((innerArray) =>
      innerArray.map((id) => sales.find((sale) => sale.id === id)!)
    );
  }
}
