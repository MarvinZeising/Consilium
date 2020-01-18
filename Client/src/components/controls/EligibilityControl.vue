<template>
  <div>
    <v-card-text
      class="pb-0"
    >
      <span
        class="subtitle-1"
        v-text="eligibility.category.name"
      />
    </v-card-text>

    <v-card-text class="pa-2">
      <v-layout wrap>

        <PermissionControl
          :model="eligibilityModel"
          translationPath="project.role.category"
          @change="(value) => eligibility.setPermissionModel('shifts', value)"
        />

        <v-flex xs12 sm6 md4>
          <v-switch
            v-model="isTeamCaptain"
            :label="$t(`project.role.teamCaptain`)"
            color="primary"
            inset
            hide-details
            :disabled="isPermissionNone"
            @change="toggleTeamCaptain"
          />
        </v-flex>

        <v-flex xs12 sm6 md4>
          <v-switch
            v-model="isSubstituteCaptain"
            :label="$t(`project.role.substituteCaptain`)"
            color="primary"
            inset
            hide-details
            :disabled="isPermissionNone"
            @change="toggleSubstituteCaptain"
          />
        </v-flex>

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

@Component({
  components: {
    PermissionControl,
  },
})
export default class EligibilityControl extends Vue {

  @Prop(Eligibility)
  private readonly eligibility?: Eligibility

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

  private created() {
    if (this.eligibility) {
      this.eligibilityModel = this.eligibility.getPermissionModel('shifts')
    }
  }

  // TODO: replace switches with a select
  private toggleTeamCaptain(value: boolean) {
    if (this.eligibility) {
      this.eligibility.isTeamCaptain = value

      if (value) {
        this.eligibility.isSubstituteCaptain = false
      }
    }
  }

  private toggleSubstituteCaptain(value: boolean) {
    if (this.eligibility) {
      this.eligibility.isSubstituteCaptain = value

      if (value) {
        this.eligibility.isTeamCaptain = false
      }
    }
  }

}
</script>
