<template>
    <div class="row">
        <div class="col-12" v-if="loaded">
            <Tree v-bind:nodes="tree.entries()" :nodeComponent="nodeComponent"></Tree>
        </div>
        <div class="col-12" v-else>
            <Spinner></Spinner>
        </div>
    </div>
</template>

<script src="@/components/SaveFile/Objects/Objects.js"></script>

<style lang="sass" scoped src="@/components/SaveFile/Objects/Objects.sass"></style>
