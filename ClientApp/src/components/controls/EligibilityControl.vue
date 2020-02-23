<template>
  <div>
    <v-card-text class="pb-0 subtitle-1">
      <span v-if="eligibility.role">{{ eligibility.role.name }}</span>
      <span v-if="eligibility.category">{{ eligibility.category.name }}</span>
    </v-card-text>

    <v-card-text class="pa-2">
      <v-layout wrap>
        <PermissionControl
          :model="eligibilityModel"
          translationPath="project.role.category"
          :showDescription="showDescription"
          @change="(value) => eligibility.setPermissionModel('shifts', value)"
        />

        <SelectControl
          :model="leadershipModel"
          label="project.role.leadership.leadership"
          :description="showDescription ? 'project.role.leadership.description' : ''"
          itemString="project.role.leadership."
          :values="['member','captain','substituteCaptain']"
          @change="(value) => eligibility.setPermissionModel('shifts', value)"
        />
      </v-layout>
    </v-card-text>
  </div>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import i18n from '../../i18n'
import { Eligibility } from '../../models'
import PermissionControl from './PermissionControl.vue'
import SelectControl from './SelectControl.vue'

@Component({
  components: {
    PermissionControl,
    SelectControl,
  },
})
export default class EligibilityControl extends Vue {
  @Prop(Eligibility)
  private readonly eligibility?: Eligibility

  @Prop(Boolean)
  private readonly showDescription?: boolean

  private get isPermissionNone() {
    return this.eligibilityModel.value === 'none'
  }

  private get isTeamCaptain() {
    return !this.isPermissionNone && this.eligibility?.isTeamCaptain
  }

  private get isSubstituteCaptain() {
    return !this.isPermissionNone && this.eligibility?.isSubstituteCaptain
  }

  private eligibilityModel = { value: 'none' }
  private leadershipModel = { value: 'member' }

  private created() {
    if (this.eligibility) {
      this.eligibilityModel = this.eligibility.getPermissionModel('shifts')
      this.leadershipModel = { value: 'member' }
    }
  }

  private setLeadership(value: string) {
    if (this.eligibility) {
      this.eligibility.isTeamCaptain = value === 'captain'
      this.eligibility.isSubstituteCaptain = value === 'substituteCaptain'
    }
  }
}
</script>
