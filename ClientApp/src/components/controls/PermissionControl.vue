<template>
  <v-flex
    xs12 sm6 md4
    class="pa-2"
  >
    <p v-t="translationPath + 'Description'" />
    <v-select
      v-model="model.value"
      :items="permissionValues"
      item-value="value"
      :label="$tc(translationPath, 2)"
      hide-details
      filled
      required
      @change="emitChange"
    >
      <template v-slot:selection="{ item }">
        <span>{{ $t('core.' + item.value) }}</span>
      </template>
      <template v-slot:item="{ item }">
        <span>{{ $t('core.' + item.value) }}</span>
      </template>
    </v-select>
  </v-flex>
</template>

<script lang="ts">
import { Vue, Component, Watch, Prop, Emit } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'

@Component
export default class PermissionControl extends Vue {

  @Prop(Object)
  private readonly model?: { value: string }

  @Prop(String)
  private readonly translationPath?: string

  private permissionValues: any[] = [ 'none', 'read', 'write' ].map((value) => {
    return { value }
  })

  @Emit('change')
  private emitChange() {
    return this.model?.value
  }

}
</script>
